import React from "react";
import { SegmentedButtons } from 'react-native-paper';
import CalendarPicker from 'react-native-calendar-picker';
import { View, StyleSheet, Text } from "react-native";


export default function CalendarContainer(props) {
    const [selectedButton, setSelectedButton] = React.useState('');
    
    return (
        <View>
            <SegmentedButtons
                onValueChange={setSelectedButton}
                value={selectedButton}
                density="medium"
                style={styles.group}
                buttons={[
                    {
                        style: styles.button,
                        value: 'month',
                        label: 'Month',
                    },
                    {
                        style: styles.button,
                        value: 'week',
                        label: 'Week',
                    },
                    {
                        style: styles.button,
                        value: 'hide',
                        label: 'Hide',
                    },
                ]}
            />

            {(selectedButton === "month" || selectedButton === "week") && 
                <View style={styles.calendarContainer}>
                    <CalendarPicker
                        showDayStragglers
                        //minDate={minDate} 
                        //maxDate={maxDate}
                        //startFromMonday={false}
                        onDateChange={props.onDateChange}
                    />
                </View>
            }
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