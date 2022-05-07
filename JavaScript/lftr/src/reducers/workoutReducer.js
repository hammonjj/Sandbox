import * as actionTypes from "../actions/types";

const initialState = {
    workout: {},
    workouts: []
}

export default function(state = initialState, action) {
    switch (action.type) {
        default:
            return state;
    }
}
