"use strict";

var React = require("react");
var Router = require("react-router");
var DefaultRoute = Router.DefaultRoute;
var Route = Router.Route;
var NotFoundRoute = Router.NotFoundRoute;

var routes = (
    <Route name="app" path="/" handler={require("./components/app")}>
        <DefaultRoute handler={require("./components/homePage")} />
        <NotFoundRoute handler={require("./components/notFoundPage")} />
        <Route name="authorPage" handler={require("./components/authors/authorPage")} />
        <Route name="about" path="author" handler={require("./components/about/aboutPage")} />
        <Route name="authorBiography" path="author/:id" handler={require("./components/authors/authorBiography")} />
        <Route name="addAuthor" handler={require("./components/authors/manageAuthorPage")} />
    </Route>
);

module.exports = routes;