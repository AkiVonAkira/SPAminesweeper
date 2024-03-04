import React, { useState, useEffect, useRef } from "react";
import * as signalR from "@microsoft/signalr";
import axios from "axios";
import styled from "styled-components";
import authService from "../api-authorization/AuthorizeService";
import { Button, Input } from '../Global/GlobalStyles';

const ChatContainer = styled.div`
  max-height: 100%;
  overflow: hidden;
  max-width: 100%;
`;
const ChatTextContainer = styled.div`
  display: flex;
  flex-direction: column-reverse;
  max-height: 80vh;
  overflow:scroll;
`;

const ChatHeader = styled.h1`
  color: #333;
`;

const FormGroup = styled.div`
  display: flex;
  flex-direction:row;
  flex-wrap: nowrap;
  gap: 1em;
`;

const Label = styled.label`
  display: block;
  flex-grow: 5;
`;

const MessageList = styled.ul`
  list-style-type: none;
  padding: 0;
`;

const MessageItem = styled.li`
  margin-bottom: 0.5em;
`;

const Chathub = () => {
  const [nickName, setNickname] = useState('');
  const [message, setMessage] = useState('');
  const [messages, setMessages] = useState([]);
  const [isConnected, setIsHubConnected] = useState(false);

  const hubConnection = useRef(null);

  const fetchNickName = async () => {
    try {
      const accessToken = await authService.getAccessToken();
      const headers = {
        Authorization: `Bearer ${accessToken}`,
      };
      const response = await axios.get("/api/user/getuser", {
        headers: headers,
      });
      setNickname(response.data.nickname);
    } catch (error) {
      console.error("Error fetching user:", error);
    }
  };

  useEffect(() => {
    fetchNickName();

    const storedGlobalChatHistory = JSON.parse(localStorage.getItem('globalChatHistory')) || [];
    setMessages(storedGlobalChatHistory);

    const startHubConnection = async () => {
      const newConnection = new signalR.HubConnectionBuilder()
        .withUrl("/chathub")
        .build();

      hubConnection.current = newConnection;
      setIsHubConnected(true);

      try {
        await newConnection.start();
        console.log("SignalR Connected");
        setIsHubConnected(true);

        newConnection.on("ReceiveMessage", (receivedUser, receivedMessage) => {
          console.log("Received user:", receivedUser);
          const now = new Date();
          const formattedTime = `${now.getHours().toString().padStart(2, '0')}:${now.getMinutes().toString().padStart(2, '0')}`;

          const newMessage = {
            user: receivedUser,
            message: receivedMessage,
            timestamp: formattedTime
          };

          setMessages(prevMessages => {
            const updatedMessages = [...prevMessages, newMessage];
            localStorage.setItem('globalChatHistory', JSON.stringify(updatedMessages));
            return updatedMessages;
          });
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

  const send = () => {
    if (hubConnection.current) {
      hubConnection.current
        .invoke("SendMessage", nickName, message)
        .catch((err) => console.error(err));
    }
  };

  if (!isConnected) {
    return (
      <ChatContainer>
        <ChatHeader>Global Chat</ChatHeader>

        <ChatTextContainer>
          <MessageList>
            {messages.map((msg, index) => (
              <MessageItem key={index}>
                <hr />
                <div>
                  <strong>{msg.user}</strong> {msg.timestamp}
                </div>
                <div>{msg.message}</div>
              </MessageItem>
            ))}
          </MessageList>
        </ChatTextContainer>

        <hr />

        <FormGroup>
          <Label>
            <Input
              value={message}
              pointer-events={"none"}
              placeholder="Not Signed in"
            />
          </Label>
          <Button onClick={send} disabled={!isConnected}>
            Send
          </Button>
        </FormGroup>
      </ChatContainer >
    );
  }
  else {
    return (
      <ChatContainer>
        <ChatHeader>Global Chat</ChatHeader>

        <ChatTextContainer>
          <MessageList>
            {messages.map((msg, index) => (
              <MessageItem key={index}>
                <hr />
                <div>
                  <strong>{msg.user}</strong> {msg.timestamp}
                </div>
                <div>{msg.message}</div>
              </MessageItem>
            ))}
          </MessageList>
        </ChatTextContainer>

        <hr />

        <FormGroup>
          <Label>
            <Input
              value={message}
              onChange={(e) => setMessage(e.target.value)}
              placeholder="Message"
            />
          </Label>
          <Button onClick={send} disabled={!isConnected}>
            Send
          </Button>
        </FormGroup>
      </ChatContainer >
    );
  }
};

export default Chathub;
