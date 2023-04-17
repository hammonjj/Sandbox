import { StyleSheet, View } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { MD3DarkTheme, Provider as PaperProvider } from 'react-native-paper';

import React, { useEffect, useState } from 'react';
import { Session } from '@supabase/supabase-js'
import { supabase } from './utils/initSupabase'

import HomeScreen from './screens/HomeScreen';
import SettingsScreen from './screens/SettingsScreen';
import SignInScreen from './screens/SignInScreen';
import SignUpScreen from './screens/SignUpScreen';

const queryClient = new QueryClient();
const Tab = createBottomTabNavigator();

const getIsSignedIn = () => {
  // custom logic
  return true;
};

export default function App() {
  const isSignedIn = getIsSignedIn();

  const [session, setSession] = useState<Session | null>(null)

  useEffect(() => {
    supabase.auth.getSession().then(({ data: { session } }) => {
      setSession(session)
    })

    supabase.auth.onAuthStateChange((_event, session) => {
      setSession(session)
    })
  }, []);

  return (
    <View style={styles.container}>
    <QueryClientProvider client={queryClient}>
      <PaperProvider theme={MD3DarkTheme}>
        <NavigationContainer>
          <Tab.Navigator>
            {session ? (
              <>
                <Tab.Screen name="Home" component={HomeScreen} />
                <Tab.Screen name="Settings" component={SettingsScreen} />
              </>
            ) : (
              <>
                <Tab.Screen name="SignIn" component={SignInScreen} />
                <Tab.Screen name="SignUp" component={SignUpScreen} />
              </>
            )}
          </Tab.Navigator>
        </NavigationContainer>
      </PaperProvider>
    </QueryClientProvider>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
})
