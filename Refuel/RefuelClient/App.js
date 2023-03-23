import { StyleSheet } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import Home from './components/Home';
import Metrics from './components/Metrics';
import Settings from './components/Settings';
import { Ionicons } from '@expo/vector-icons';
import Toast from 'react-native-toast-message';

const Tab = createBottomTabNavigator();

export default function App() {
  return (
    <>
      <NavigationContainer>
        <Tab.Navigator
          screenOptions={({ route }) => ({
            tabBarIcon: ({ focused, color, size }) => {
              if (route.name === 'Refuel') {
                return (
                  <Ionicons
                    name={
                      focused
                        ? 'ios-information-circle'
                        : 'ios-information-circle-outline'
                    }
                    size={size}
                    color={color}
                  />
                );
              } else if (route.name === 'Settings') {
                return (<Ionicons name='settings' size={size} color={color} />);
              } else if (route.name === 'Metrics') {
                return (<Ionicons name='analytics' size={size} color={color} />);
            }
            },
            tabBarInactiveTintColor: 'gray',
            tabBarActiveTintColor: 'tomato',
          })}
        >
          <Tab.Screen name="Refuel" component={Home} />
          <Tab.Screen name="Metrics" component={Metrics} />
          <Tab.Screen name="Settings" component={Settings} />
        </Tab.Navigator>
      </NavigationContainer>
      <Toast 
        position='bottom'
        onPress={() => {Toast.hide()}} />
    </>
  );
}