import AsyncStorage from '@react-native-async-storage/async-storage';
import { API_URL } from './Constants';

export async function storeLocalValue(key: string, value: string): Promise<Boolean> {
    let success = true;
    try {
        await AsyncStorage.setItem(key, value);
    } catch (error) {
        success = false;
    }

    return success;
}

export async function getLocalValue(key: string): Promise<string> {
    let value: string | null = '';
    try {
        value = await AsyncStorage.getItem(key);
    } catch (error) {
        value = '';
    }

    if(value === null) {
        value = '';
    }

    return value;
}

export async function retrieveUserName(): Promise<string> {
    return getLocalValue('username');
}

export async function retrievePassword(): Promise<string> {  
    return getLocalValue('password');
}

export async function saveUserName(username: string): Promise<Boolean> {
    return storeLocalValue('username', username);
}

export async function savePassword(password: string): Promise<Boolean> {
    return storeLocalValue('password', password);
}

export function retrieveServerUrl(): string {
    return API_URL;
    //return getLocalValue('serverUrl');
}

export async function saveServerUrl(url: string): Promise<Boolean> {
    return storeLocalValue('serverUrl', url);
}