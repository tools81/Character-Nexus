import * as React from 'react';
import Container from 'react-bootstrap/Container';
import NavMenu from './NavMenu';

interface LayoutProps {
  children?: React.ReactNode;
}

export default class Layout extends React.PureComponent<LayoutProps> {
    public render() {
        return (
            <React.Fragment>
                <NavMenu />
                <Container>
                    {this.props.children}
                </Container>
            </React.Fragment>
        );
    }
}
