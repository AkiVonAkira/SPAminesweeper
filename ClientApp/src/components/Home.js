import React, { Component } from "react";
import { FetchBoard } from "./FetchBoard";
import styled from "styled-components";

const GameContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background-color: #c2c2c2;
  border-top: 4px #fefefe solid;
  border-left: 4px #fefefe solid;
  border-bottom: 4px #787976 solid;
  border-right: 4px #787976 solid;
  padding: 1em;
  gap: 1em;
`;

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <GameContainer>
        <button></button>
        <input aria-label="gridSize"></input>
        <FetchBoard />
      </GameContainer>
    );
  }
}
