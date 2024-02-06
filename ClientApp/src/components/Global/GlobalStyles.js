import { createGlobalStyle } from "styled-components";
import styled from "styled-components";

export const RootStyles = createGlobalStyle`
    :root{
        --text: #120a0a;
        --background: #f8f2f3;
        --primary: #c2c2c2;
        --secondary: #b3c99d;
        --accent: #85b980;
    }
    :root[data-theme="light"] {
        --text: #120a0a;
        --background: #f8f2f3;
        --primary: #c2c2c2;
        --secondary: #b3c99d;
        --accent: #85b980;
    }
    :root[data-theme="dark"] {
        --text: #f6efef;
        --background: #0d0708;
        --primary: #3d3d3d;
        --secondary: #4d6336;
        --accent: #4b8047;
    }
`;

export const Button = styled.button`
  background-color: var(--secondary);
  padding: 0.5em 1em;
  margin: 0;
  border: 0;
  border-radius: 0.5em;
  height: 3em;
  flex-grow: 1;

  &:hover {
    box-shadow: 2px 2px 4px rgba(0, 0, 0, 0.4);
  }
`;

export const Input = styled.input`
  display: flex;
  /* background-color: var(--primary); */
  border: 0.25em var(--primary) solid;
  border-radius: 0.5em;
  padding: 0.5em;
  flex-grow: 1;

  &:focus{
    border: 0.25em var(--accent) solid;
  }
`;