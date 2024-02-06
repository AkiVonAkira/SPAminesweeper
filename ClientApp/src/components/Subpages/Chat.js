import React, { useState, useEffect, useRef } from "react";
import * as signalR from "@microsoft/signalr";
import axios from "axios";
import styled from "styled-components";
import authService from "../api-authorization/AuthorizeService";
import { Button, Input } from '../Global/GlobalStyles';

const ChatContainer = styled.div`
  text-align: center;
  margin: 2em;
  min-height: 100%;
`;

const ChatHeader = styled.h1`
  color: #333;
`;

const FormGroup = styled.div`
  margin-bottom: 1em;
`;

const Label = styled.label`
  display: block;
  margin-bottom: 0.5em;
`;
const MessageList = styled.ul`
  list-style-type: none;
  padding: 0;
`;

const MessageItem = styled.li`
  margin-bottom: 0.5em;
`;

const Chathub = () => {
  const [nickName, setNickName] = useState("");
  const [message, setMessage] = useState("");
  const [messages, setMessages] = useState([]);
  const [isConnected, setIsHubConnected] = useState(null);

  const hubConnection = useRef(null);

  useEffect(() => {
    fetchNickName();
    const startHubConnection = async () => {
      const newConnection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();

      hubConnection.current = newConnection;
      setIsHubConnected(true);

      try {
        await newConnection.start();
        console.log("SignalR Connected");
        setIsHubConnected(true);

        newConnection.on("ReceiveMessage", (receivedUser, receivedMessage) => {
          setMessages((prevMessages) => [
            ...prevMessages,
            `${receivedUser} says: ${receivedMessage}`
          ]);
        });
      } catch (err) {
        console.error(err);
      }

      return () => {
        if (newConnection) {
          newConnection.stop();
        }
      };
    };

    startHubConnection();
  }, []);

  const fetchNickName = async () => {
    try {
      const accessToken = await authService.getAccessToken();
      const headers = {
        Authorization: `Bearer ${accessToken}`
      };
      const response = await axios.get("/api/user/getuser", {
        headers: headers
      });
      setNickName(response.data.nickName);
    } catch (error) {
      console.error("Error fetching user:", error);
    }
  };

  const send = async () => {
    if (hubConnection.current) {
      await hubConnection.current
        .invoke("SendMessage", nickName, message)
        .catch((err) => console.error(err));
    }
  };

  return (
    <ChatContainer>
      <ChatHeader>Chat</ChatHeader>
      <FormGroup>
        <Label>
          Message:
          <Input
            value={message}
            onChange={(e) => setMessage(e.target.value)}
            size="50"
          />
        </Label>
      </FormGroup>

      <Button onClick={send} disabled={!isConnected}>
        Send
      </Button>

      <hr />

      <MessageList>
        {messages.map((msg, index) => (
          <MessageItem key={index}>{msg}</MessageItem>
        ))}
      </MessageList>
    </ChatContainer>
  );
};

export default Chathub;
