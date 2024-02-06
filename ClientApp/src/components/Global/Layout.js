import React, { Component } from 'react';
import { Container as BootstrapContainer } from 'reactstrap';
import { NavMenu } from './NavMenu';
import styled from 'styled-components';

const MainContainer = styled(BootstrapContainer)`
  width: 100%;
  max-width: 100%;
  min-height: 100%;
`;

const HomeContainer = styled.div`
  min-height: 100vh;
  background-color: var(--background);
`;

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <HomeContainer>
        <NavMenu />
        <MainContainer tag="main">
          {this.props.children}
        </MainContainer>
      </HomeContainer>
    );
  }
}
