import { Picker } from '@react-native-picker/picker';
import { useState } from 'react';
import { Button, Text, TextInput, View } from 'react-native';
import Toast from 'react-native-toast-message';
import { StyleSheet } from 'react-native';
import { Keyboard } from 'react-native';
import { Pressable } from 'react-native';

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

    const [selectedVehicle, setSelectedVehicle] = useState("");
    //Vehicle
    //  - API
    //Date
    //  - Auto Filled
    //GallonsPumped
    //PricerPerGallon
    //MilesDriven
    function onSubmitRefill() {
        //Get Long/Lat
        Keyboard.dismiss();
        Toast.show({
            type: "success",
            text1: "Submitting Refueling Successfully"
        });
    }

    return (
        <View style={styles.gasInputContainer}>
            <Text style={styles.gasInputLabel}>Date:</Text>
            <TextInput style={styles.gasInputTextInput} id="date" />
            
            <Text style={styles.gasInputLabel}>Vehicle:</Text>
            <Picker 
                style={styles.gasInputVehicleDropdown}
                selectedValue={selectedVehicle}
                onValueChange={(value, index) => setSelectedVehicle(value)}    
            >
                <Picker.Item label="" value="" />
                <Picker.Item label="Hyundai Elentra" value="Hyundai Elentra" />
            </Picker>
            
            <Text style={styles.gasInputLabel}>Gallons:</Text>
            <TextInput 
                style={styles.gasInputTextInput} 
                keyboardType="numeric" 
                id="gallonsPumped"
            />

            <Text style={styles.gasInputLabel}>PPG:</Text>
            <TextInput  style={styles.gasInputTextInput} keyboardType="numeric" id="pricePerGallon" />

            <Text style={styles.gasInputLabel}>Miles Driven:</Text>
            <TextInput  style={styles.gasInputTextInput} keyboardType="numeric" id="milesDriven" />

            <Button color="#841584" title="Submit" onPress={onSubmitRefill} />
        </View>
    );
}

const styles = StyleSheet.create<any>({
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
        width: "25%"
    },
    gasInputTextInput: {
        height: 50,
        width: "75%",
        borderRadius: 50,
        backgroundColor: '#DCDCDC',
    },
    gasInputVehicleDropdown: {
        width: "75%",
        borderRadius: 50,
        backgroundColor: '#DCDCDC'
    },
    gasInputSubmit: {
        width: "75%",
        left: "50%"
    }
});

export default Home;