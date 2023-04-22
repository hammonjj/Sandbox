import React from "react";
import { View, StyleSheet, Text } from "react-native";
import { useTheme } from "react-native-paper";
import CalendarContainer from "../components/CalendarContainer";
import AppleHealthKit, {
    HealthValue,
    HealthKitPermissions,
    HealthInputOptions,
  } from 'react-native-health'

export default function HomeScreen() {
    const theme = useTheme();

    function onDateChange(date: Date) {
        //Date is epoch time
        const formattedDate = new Date(date);
        console.log("Callback Date: " + formattedDate.toDateString());


        let options: HealthInputOptions = {
            //unit: 'pound', // optional; default 'pound'
            startDate: new Date(2023, 4, 1).toISOString(), // required
            endDate: new Date().toISOString(), // optional; default now
            ascending: false, // optional; default false
            limit: 10, // optional; default no limit
          }

          AppleHealthKit.getWeightSamples(
            options,
            (err: Object, results: Array<HealthValue>) => {
              if (err) {
                console.log('error getting weight samples: ', err)
                return
              }
              console.log("Weight Samples: " + results)
            },
          )
    }

    return (
        <View>
            <CalendarContainer onDateChange={onDateChange} />
            <View>
                <Text>Activity Container</Text>
            </View>
        </View>
    );
}

const styles = StyleSheet.create({
    button: {
      flex: 1,
      //borderRadius: 5,
    },
    group: { 
        paddingHorizontal: 20, 
        paddingTop: 8,
        paddingBottom: 8,
        justifyContent: 'center' 
    },
    calendarContainer: {
        backgroundColor: "#4d4357",
    },
    activityContainer: {
        flex: 1,
        justifyContent: 'center',
    }
  });