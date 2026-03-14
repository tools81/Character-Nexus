import * as React from 'react';
import { Container, Navbar, NavbarBrand, NavItem, NavLink } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import '../NavMenu.css';

export default class NavMenu extends React.PureComponent<{}, { isOpen: boolean }> {
    public state = {
        isOpen: false
    };

    public render() {
        return (
          <header>
            <Navbar bg="dark" data-bs-theme="dark">
              <Container>
                <Navbar.Brand href={"#home"}>
                  <img
                    alt=""
                    src="/Heavy Tool logo transparent.png"
                    width="30"
                    height="30"
                    className="logo"
                  />{' '}
                  <img
                    alt="Character Nexus"
                    src="/Title.png"
                    height="30"
                    className="title"
                  />
                </Navbar.Brand>
              </Container>
            </Navbar>
          </header>
        );
    }

    private toggle = () => {
        this.setState({
            isOpen: !this.state.isOpen
        });
    }
}
