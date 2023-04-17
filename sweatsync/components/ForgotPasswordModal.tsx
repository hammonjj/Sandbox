import { useState } from "react";
import { View, Text } from "react-native";
import { Button, Modal } from "react-native-paper";

interface IProps {
    visible: boolean;
}

export default function ForgotPasswordModal(props: IProps) {
    //contentContainerStyle={containerStyle}
    const [visible, setVisible] = useState(true);
    return (  
        <Modal visible={props.visible} onDismiss={() => setVisible(false)} >
            <View><Text>Forgot Password Modal</Text>
            <Button onPress={() => setVisible(false)}>Close</Button></View>
        </Modal>
    );

}