import { useState } from 'react';
import { StyleSheet, Text, View } from 'react-native';
import DropDownPicker from 'react-native-dropdown-picker';
import { Table, Row, Rows } from 'react-native-table-component';

export default function Metrics() {
    const [selectedVehicle, setSelectedVehicle] = useState(null);
    const [vehicles, setVehicles] = useState([
        {label: "Hyundai Elantra", value: "Hyundai Elantra"}
    ]);
    const [open, setOpen] = useState(false);

    return (
        <View style={styles.mainContainer}>
            <DropDownPicker 
                    style={styles.vehiclePicker}
                    open={open}
                    setOpen={setOpen}
                    value={selectedVehicle}
                    setValue={setSelectedVehicle}
                    items={vehicles}
                    setItems={setVehicles} />
            <View style={styles.tableStyle}>
                <Table borderStyle={{borderWidth: 1, borderColor: '#000000'}}>
                    <Row data={['Date', 'Gallons', 'PPG', 'Miles', 'MPG']} textStyle={styles.tableText} />
                    <Rows data={[
                        ['1/1/2023', '15.35', '$3.23', '352', '32.2'], 
                        ['1/1/2023', '15.35', '$3.23', '352', '32.2'], 
                        ['1/1/2023', '15.35', '$3.23', '352', '32.2']]} textStyle={styles.tableText} />
                </Table>
            </View>
        </View>
    );
}

const styles = StyleSheet.create<any>({
    vehiclePicker: {
        backgroundColor: '#DCDCDC',
        width: "65%",
        marginHorizontal: 60,
    },
    tableStyle: {
        marginVertical: 10,
        marginHorizontal: 10,
        backgroundColor: '#DCDCDC',
    },
    tableHead: {

    },
    tableText: {
        margin: 6,
    },
    mainContainer: {
        marginTop: 10,
    }
});