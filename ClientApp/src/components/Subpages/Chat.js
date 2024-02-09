import React, { useState, useEffect, useRef } from "react";
import * as signalR from "@microsoft/signalr";
import axios from "axios";
import styled from "styled-components";
import authService from "../api-authorization/AuthorizeService";

const ChatContainer = styled.div`
  text-align: center;
  margin: 2em;
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

const Input = styled.input`
  display: block;
  width: 100%;
  padding: 0.5em;
  border: 0.25em #787976 solid;
  border-radius: 0.5em;
`;

const Button = styled.button`
  background-color: #c2c2c2;
  padding: 0.5em 1em;
  margin: 0;
  border: 0.25em red solid;
  border-radius: 0.5em;
  height: 3em;
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
    const [isConnected, setIsHubConnected] = useState(false);

    const hubConnection = useRef(false);

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
                    const now = new Date();
                    const hours = now.getHours().toString().padStart(2, '0');
                    const minutes = now.getMinutes().toString().padStart(2, '0');
                    const formattedTime = `${hours}:${minutes}`;
                    const newMessage = `${receivedUser} says:   ${receivedMessage} Timestamp:   ${formattedTime}`;
                    setMessages(prevMessages => [...prevMessages, newMessage]);
                    // Spara nytt meddelande till global chatt-historik
                    localStorage.setItem('globalChatHistory', JSON.stringify([...messages, newMessage]));
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
                Authorization: `Bearer ${accessToken}`,
            };
            const response = await axios.get("/api/user/getuser", {
                headers: headers,
            });
            setNickName(response.data.nickName);
        } catch (error) {
            console.error("Error fetching user:", error);
        }
    };

    const send = () => {
        if (hubConnection.current) {
            hubConnection.current
                .invoke("SendMessage", nickName, message)
                .catch((err) => console.error(err));
        }
    };

    return (
        <ChatContainer>
            <ChatHeader>Global Chat</ChatHeader>

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
        </ChatContainer >
    );
};

export default Chathub;
