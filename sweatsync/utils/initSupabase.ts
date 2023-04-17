import * as SecureStore from "expo-secure-store";
import { createClient } from '@supabase/supabase-js'
import { setupURLPolyfill } from 'react-native-url-polyfill'

setupURLPolyfill();

const ExpoSecureStoreAdapter = {
  getItem: (key: string) => {
    return SecureStore.getItemAsync(key);
  },
  setItem: (key: string, value: string) => {
    SecureStore.setItemAsync(key, value);
  },
  removeItem: (key: string) => {
    SecureStore.deleteItemAsync(key);
  },
};

const supabaseUrl = "https://akiwhwohjzyudzdxzcmb.supabase.co"
const supabaseAnonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImFraXdod29oanp5dWR6ZHh6Y21iIiwicm9sZSI6ImFub24iLCJpYXQiOjE2ODE2NzkxMjYsImV4cCI6MTk5NzI1NTEyNn0.RPjgckFLosjhAll7fkTG48uJLmrRsVOUYMMGNF-3L2o"

export const supabase = createClient(supabaseUrl, supabaseAnonKey, {
  auth: {
    storage: ExpoSecureStoreAdapter as any,
    autoRefreshToken: true,
    persistSession: true,
    detectSessionInUrl: false,
  },
})