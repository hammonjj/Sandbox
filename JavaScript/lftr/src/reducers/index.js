// Set up your root reducer here...
import { combineReducers } from 'redux';
import workoutReducer from "./workoutReducer";

export default combineReducers({
   workout: workoutReducer
});