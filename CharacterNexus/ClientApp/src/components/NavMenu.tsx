import * as React from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import '../NavMenu.css';

export default class NavMenu extends React.PureComponent<{}, { isOpen: boolean }> {
    public state = {
        isOpen: false
    };

    public render() {
        return (
          <header>
            <Navbar
              className="custom-navbar navbar-expand-sm navbar-toggleable-sm border-bottom mb-3"
              dark
            >
              <Container>
                <NavbarBrand>
                  <img
                    alt="logo"
                    src="/Heavy Tool logo transparent.png"
                    className="logo"
                  />
                </NavbarBrand>
                <NavbarBrand tag={Link} to="/">
                  Character Nexus
                </NavbarBrand>
                <NavbarToggler onClick={this.toggle} className="mr-2" />
                <Collapse
                  className="d-sm-inline-flex flex-sm-row-reverse"
                  isOpen={this.state.isOpen}
                  navbar
                >
                  <ul className="navbar-nav flex-grow">
                    <NavItem>
                      <NavLink tag={Link} className="text-light" to="/">
                        Rulesets
                      </NavLink>
                    </NavItem>
                  </ul>
                </Collapse>
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
