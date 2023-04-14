import axios from 'axios';
import base64 from 'react-native-base64';
import { API_URL } from './constants';

export type Method = 'get' | 'post';

export type Refuel = {
    vehicleId: number;
    date: Date;
    tripMiles: number;
    gallons: number;
    milesPerGallon: number;
    pricePerGallon: number;
    latitude: number;
    longitude: number;
};

export const fetchVehicles = async () => {
    try {
        let vehiclesApiCall = await getVehicles();
        if(!vehiclesApiCall) {
            return [{label: "", value: ""}];
        }

        let vehicleItems = vehiclesApiCall.data.map((vehicle: any) => {
            return {label: vehicle.make + ' ' + vehicle.model, value: vehicle.id}
        });

        vehicleItems.unshift({label: "", value: ""});
        return vehicleItems;
    } catch (error) {
        console.log(`Exception Caught: ${error}`);
    }
}

export const submitRefuel = (refuel: Refuel) => {
    return ApiPostRequest('/refuel/fillups/create', refuel);
};

const getVehicles = async () => {
    try {
        const request = await ApiGetRequest(`/refuel/vehicles`);   
        return request; 
    } catch (error) {
        console.log(error);
        return null;
    }
};

export const getRefuels = async (vehicleId: number) => {
    try {
        const request = await ApiGetRequest(`/refuel/fillups/${vehicleId}`);
        return request;
    } catch (error) {
        console.log(error);
    }
};

const ApiPostRequest = async (path: string, data: object) => {
    const serverUrl = API_URL;
    const username = await retrieveUserName();
    const password = await retrievePassword();

    if(!username || !password) {
        return null;
    }

    const authHeader = 'Basic ' + base64.encode(`${username}:${password}`);

    const response = await axios.post(serverUrl + path, data,
        {
            headers: { 'Authorization': authHeader, Accept: 'application/json' },
        },
    );

    return response.data;
};

const ApiGetRequest = async (path: string) => {
    const serverUrl = retrieveServerUrl();
    const username = await retrieveUserName();
    const password = await retrievePassword();

    if(!username || !password) {
        return null;
    }
    
    const authHeader = 'Basic ' + base64.encode(`${username}:${password}`);

    const response = await axios.get(serverUrl + path,
        {
            headers: { 'Authorization': authHeader, Accept: 'application/json' }
        },
    );

    return response.data;
};