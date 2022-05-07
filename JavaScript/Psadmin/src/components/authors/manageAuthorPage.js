"use strict";

var React = require("react");
var AuthorForm = require("./authorForm");
var AuthorApi = require("../../api/authorApi");
var Router = require("react-router");
var toastr = require("toastr");

var ManageAuthorPage = React.createClass({
    mixins: [
        Router.Navigation
    ],

    statics: {
      willTransitionFrom: function(transition, component){
          if(component.state.dirty && !confirm("Leave without saving?")){
            transition.abort();
          }
      }  
    },

    getInitialState: function() {
        return{
            author: { id: "", firstName: "", lastName: "" },
            errors: {},
            dirty: false
        }
    },

    setAuthorState: function(event){
        this.setState({ dirty: true })
        var field = event.target.name;
        var value = event.target.value;
        this.state.author[field] = value;
        return this.setState({author: this.state.author})
    },

    authorFormIsValid: function(){
        var formIsValid = true;
        this.state.errors = {};

        if(this.state.author.firstName.length < 1){
            formIsValid = false;
            this.state.errors.firstName = "First name cannot be empty"
        }
        if(this.state.author.lastName.length < 1){
            formIsValid = false;
            this.state.errors.lastName = "Last name cannot be empty"
        }

        this.setState({errors: this.state.errors});
        return formIsValid;
    },

    saveAuthor: function(event){
        event.preventDefault();

        if(!this.authorFormIsValid()){
            return;
        }

        AuthorApi.saveAuthor(this.state.author);
        this.setState({ dirty: false });
        toastr.success("Author Saved");
        this.transitionTo("authorPage");
    },

    render: function(){
        return(
            <div>
                <AuthorForm author={this.state.author}
                onChange={this.setAuthorState}
                onSave={this.saveAuthor} 
                errors={this.state.errors} />
            </div>
        );
    }
});

module.exports = ManageAuthorPage;