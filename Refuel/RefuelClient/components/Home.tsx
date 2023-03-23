import { useState } from 'react';
import { Button, Text, TextInput, View, Keyboard, TouchableWithoutFeedback, StyleSheet } from 'react-native';
import Toast from 'react-native-toast-message';
import DropDownPicker from 'react-native-dropdown-picker';
import DateTimePicker, { DateTimePickerEvent } from '@react-native-community/datetimepicker';

const Home = () => {
    const [requestParams, setRequestParams] = useState({
        vehicle: 0,
        date: "",
        gallonsPumped: 0,
        pricePerGallon: 0.00,
        milesDriven: 0.00,
        stationLat: 0, //Auto
        stationLong: 0 //Auto
    });

    const [date, setDate] = useState<Date>(new Date());
    const [selectedVehicle, setSelectedVehicle] = useState(null);
    const [vehicles, setVehicles] = useState([
        {label: "Hyundai Elantra", value: "Hyundai Elantra"}
    ]);
    const [dateOpen, setDateOpen] = useState(false);
    const [vehicleOpen, setVehicleOpen] = useState(false);

    function onDateSelected(event: DateTimePickerEvent, value: Date | undefined) {
        if(value === undefined) {
            return;
        }
        
        setDate(value);
        setDateOpen(false);
      };

    function onSubmitRefill() {
        //Get Long/Lat
        Keyboard.dismiss();
        Toast.show({
            type: "success",
            text1: "Submitting Refueling Successfully"
        });
    }

    return (
        <TouchableWithoutFeedback onPress={Keyboard.dismiss} accessible={false}>
            <View style={styles.gasInputContainer}>
                <DropDownPicker 
                        style={styles.vehiclePicker}
                        open={vehicleOpen}
                        setOpen={setVehicleOpen}
                        value={selectedVehicle}
                        setValue={setSelectedVehicle}
                        items={vehicles}
                        setItems={setVehicles} />

                <Text style={styles.gasInputLabel}>Date:</Text>
                <DateTimePicker
                    testID="dateTimePicker"
                    value={date}
                    mode="date"
                    is24Hour={false}
                    display="default"
                    onChange={onDateSelected}
                    style={styles.datePicker}
                />
                
                <Text style={styles.gasInputLabel}>Gallons:</Text>
                <TextInput 
                    style={styles.gasInputTextInput} 
                    keyboardType="numeric" 
                    id="gallonsPumped"
                />

                <Text style={styles.gasInputLabel}>PPG:</Text>
                <TextInput  style={styles.gasInputTextInput} keyboardType="numeric" id="pricePerGallon" />

                <Text style={styles.gasInputLabel}>Trip Miles:</Text>
                <TextInput  style={styles.gasInputTextInput} keyboardType="numeric" id="milesDriven" />

                <Button color="#841584" title="Submit" onPress={onSubmitRefill} />
            </View>
        </TouchableWithoutFeedback>
    );
}

const styles = StyleSheet.create<any>({
    vehiclePicker: {
        backgroundColor: '#DCDCDC',
        width: "65%",
        marginHorizontal: 60,
    },
    datePicker: {
        marginTop: 20,
        width: "65%",
    },
    buttonStyle: {
        backgroundColor: '#DCDCDC',
    },
    gasInputContainer: {
        flex: 1,
        flexDirection: "row",
        flexWrap: "wrap",
        background: '#fff',
        alignItems: 'center',
        justifyContent: "space-evenly",

        marginTop: 10,
        marginLeft: 10,
        marginRight: 10
    },
    gasInputLabel: {
        fontSize: 20,
        width: "25%"
    },
    gasInputTextInput: {
        height: 50,
        padding: 13,
        fontSize: 20,
        width: "65%",
        borderRadius: 50,
        backgroundColor: '#DCDCDC',
        marginBottom: 5
    },
    gasInputVehicleDropdown: {
        width: "65%",
        borderRadius: 50,
        backgroundColor: '#DCDCDC',
    },
    gasInputSubmit: {
        width: "65%",
        left: "50%"
    }
});

export default Home;