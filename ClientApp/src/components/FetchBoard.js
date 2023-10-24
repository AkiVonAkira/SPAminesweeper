import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'
import axios from 'axios';

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
      <div className="minesweeper-board">
        {board.tiles.map((tile, index) => (
          <div
            key={index}
            className={`tile ${tile.isRevealed ? 'revealed' : 'hidden'}`}
          >
            {tile.isRevealed ? (
              tile.isMine ? (
                <span className="mine">💣</span>
              ) : (
                tile.adjacentMines > 0 && <span>{tile.adjacentMines}</span>
              )
            ) : (
              // eslint-disable-next-line no-undef
              <button onClick={() => handleTileClick(tile)}>
                {tile.isFlagged ? '🚩' : ''}
              </button>
            )}
          </div>
        ))}
      </div>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchBoard.renderBoard(this.state.board);

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
      method: 'post',
      url: '/api/board/createboard',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': token ? `Bearer ${token}` : '',
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
        console.error('Failed to fetch board data', response);
      }
    } catch (error) {
      console.error('Error fetching board data', error);
    }
  }
}
