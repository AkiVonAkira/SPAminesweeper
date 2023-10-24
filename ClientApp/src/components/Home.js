import React, { Component } from 'react';
import { FetchBoard } from './FetchBoard'

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
        <FetchBoard />
      </div>
    );
  }
}