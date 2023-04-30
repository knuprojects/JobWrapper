import React from 'react';
import { Route, Navigate } from 'react-router-dom';

function PrivateRoute({ element: Component, ...rest }) {
    const isAuthenticated = localStorage.getItem('token') !== null;

    return (
        <Route
            {...rest}
            element={isAuthenticated ? <Component /> : <Navigate to="/" />}
        />
    );
}

export default PrivateRoute;