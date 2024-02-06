import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import { Layout } from './components/Global/Layout';
import { RootStyles } from './components/Global/GlobalStyles';
import './custom.css';

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <RootStyles />
                <Routes>
                    {AppRoutes.map((route, index) => {
                        const { element, requireAuth, ...rest } = route;
                        return <Route key={index} {...rest} element={requireAuth ? <AuthorizeRoute {...rest} element={element} /> : element} />;
                    })}
                </Routes>
            </Layout>
        );
    }
}
