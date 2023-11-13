import React, { Component } from "react";
import { StartGame } from "./StartGame";
import styled from "styled-components";

const GameContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background-color: #c2c2c2;
  border-top: .25em #fefefe solid;
  border-left: .25em #fefefe solid;
  border-bottom: .25em #787976 solid;
  border-right: .25em #787976 solid;
  padding: 1em;
  gap: 1em;
`;

const CreateButton = styled.button`
  background-color: #c2c2c2;
  padding: .5em 1em;
  margin: 0;
  border-top: .25em #fefefe solid;
  border-left: .25em #fefefe solid;
  border-bottom: .25em #787976 solid;
  border-right: .25em #787976 solid;
`;

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <GameContainer>
        <input aria-label="gridSize"></input>
        <input aria-label="bombPercentage"></input>
        <CreateButton>Start Game</CreateButton>
        <StartGame />
      </GameContainer>
    );
  }
}
