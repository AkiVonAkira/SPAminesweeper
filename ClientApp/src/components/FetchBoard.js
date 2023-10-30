import React, { Component } from "react";
import authService from "./api-authorization/AuthorizeService";
import axios from "axios";
import styled from "styled-components";

const Board = styled.div`
  display: grid;
  grid-template-columns: repeat(var(--size), auto);
  grid-template-rows: repeat(var(--size), auto);
  gap: 0;
  padding: 0.25em;
  min-width: 10em;
  min-height: 10em;
  border-top: 4px #787976 solid;
  border-left: 4px #787976 solid;
  border-bottom: 4px #fefefe solid;
  border-right: 4px #fefefe solid;
`;
const Tile = styled.div`
  width: 2em;
  height: 2em;
  aspect-ratio: 1/1;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2em;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.4);
`;
const TileButton = styled.button`
  width: 100%;
  height: 100%;
`;

export class FetchBoard extends Component {
  static displayName = FetchBoard.name;

  constructor(props) {
    super(props);
    this.state = { board: [], loading: true };
  }

  componentDidMount() {
    this.populateBoardData();
  }

  static renderBoard(board) {
    return (
      <Board
        className="minesweeper-board"
        style={{ "--size": `${board.boardSize}` }}
      >
        {board.tiles.map((tile, index) => (
          <Tile
            key={index}
            className={`tile /*${tile.isRevealed ? "revealed" : "hidden"}*/`}
          >
            {tile.isRevealed ? (
              tile.isMine ? (
                <span className="mine">💣</span>
              ) : (
                tile.adjacentMines > 0 && <span>{tile.adjacentMines}</span>
              )
            ) : (
              // eslint-disable-next-line no-undef
              <TileButton onClick={() => handleTileClick(tile)}>
                {tile.isFlagged ? "🚩" : ""}
              </TileButton>
            )}
          </Tile>
        ))}
      </Board>
    );
  }

  render() {
    let contents = this.state.loading ? (
      <p>
        <em>Loading...</em>
      </p>
    ) : (
      FetchBoard.renderBoard(this.state.board)
    );

    return (
      <div>
        <h1 id="tableLabel">Minesweeper</h1>
        {contents}
      </div>
    );
  }

  async populateBoardData() {
    const token = await authService.getAccessToken();

    const config = {
      method: "post",
      url: "/api/board/createboard",
      headers: {
        "Content-Type": "application/json",
        Authorization: token ? `Bearer ${token}` : ""
      },
      data: {
        boardSize: 10,
        bombPercentage: 5
      }
    };

    try {
      const response = await axios(config);

      if (response.status === 200) {
        this.setState({ board: response.data, loading: false });
      } else {
        console.error("Failed to fetch board data", response);
      }
    } catch (error) {
      console.error("Error fetching board data", error);
    }
  }
}
