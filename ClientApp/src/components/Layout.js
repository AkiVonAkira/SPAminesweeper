import React, { Component } from 'react';
import { Container as BootstrapContainer } from 'reactstrap';
import { NavMenu } from './NavMenu';
import styled from 'styled-components';

const StyledContainer = styled(BootstrapContainer)`
  width: 100%;
  min-height: 100%;
`;

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <div>
        <NavMenu />
        <StyledContainer tag="main">
          {this.props.children}
        </StyledContainer>
      </div>
    );
  }
}
