"use strict";

var React = require("react");

var AuthorBiography = React.createClass({
    componentWillMount: function(){
        var authorId = this.props.params.id;
        console.log("Author ID: " + authorId);

        this.setState({id: authorId});
    },

    render: function(){
        return(
            <div>
                <h1>Author: {this.state.id}</h1>
            </div>
        );
    }
});

module.exports = AuthorBiography;