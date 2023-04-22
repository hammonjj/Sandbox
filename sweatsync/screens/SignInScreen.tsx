import React, { useState } from "react";
import { View, Alert, StyleSheet } from "react-native";
import { supabase } from "../utils/initSupabase";
import { Button, TextInput, Avatar, Portal } from "react-native-paper";
import ForgotPasswordModal from "../components/ForgotPasswordModal";

export default function SignInScreen() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [loading, setLoading] = useState(false);
    const [securePasswordEntry, setSecurePasswordEntry] = useState(true);
    const [visible, setVisible] = React.useState(false);

    async function signInWithEmail() {
        setLoading(true)
        const { error } = await supabase.auth.signInWithPassword({
          email: email,
          password: password,
        })
    
        if (error) {
            Alert.alert(error.message)
        } else {
            Alert.alert('Success', 'You are logged in')
        }
        setLoading(false)
    }

    function forgotPassword() {
        //setVisible(true);
        //const { data, error } = await supabase.auth.resetPasswordForEmail(email);
        //create modal for forgot password
    }
      
    return (
        <View style={styles.container}>
            <Portal>
                <ForgotPasswordModal visible={visible} />
            </Portal>
            <Avatar.Icon style={styles.avatar} icon="folder" size={120}/>
            <View style={[styles.verticallySpaced, styles.mt20]}>
                <TextInput
                    label="Email"
                    style={styles.textInput}
                    onChangeText={(text) => setEmail(text)}
                    value={email}
                    placeholder="email@address.com"
                    autoCapitalize={'none'}
                    secureTextEntry={false}
                />
            </View>
            <View style={styles.verticallySpaced}>
                <TextInput
                    label="Password"
                    style={styles.textInput}
                    onChangeText={(text) => setPassword(text)}
                    value={password}
                    secureTextEntry={securePasswordEntry}
                    placeholder="Password"
                    autoCapitalize={'none'}
                    right={
                        <TextInput.Icon
                          icon={securePasswordEntry ? 'eye' : 'eye-off'}
                          onPress={() => setSecurePasswordEntry(!securePasswordEntry)} />
                          }
                />

                <Button 
                    disabled={loading} 
                    onPress={forgotPassword} 
                    style={styles.forgot_button}>
                        Forgot Password?
                </Button>
            </View>
            <View style={[styles.verticallySpaced, styles.mt20]}>
                <Button 
                    disabled={loading} 
                    onPress={() => signInWithEmail()} 
                    mode="contained-tonal"
                    style={styles.button}>
                        Login
                </Button>
            </View>
        </View>
    );
}

const styles = StyleSheet.create({
    container: {
        alignItems: 'center',
    },
    button: {
        marginHorizontal: 40,
        minWidth: 300,
        
    },
    textInput: {
        borderTopLeftRadius: 40,
        borderTopRightRadius: 40,
        borderBottomLeftRadius: 40,
        borderBottomRightRadius: 40,
        minWidth: 250,
    },
    verticallySpaced: {
      paddingTop: 4,
      paddingBottom: 4,
    },
    avatar: {
        marginHorizontal: 8,
        marginTop: 110,
        marginBottom: 40,
    },
    mt20: {
      marginTop: 20,
    },
    surface: {
        padding: 8,
        height: 80,
        width: 80,
        alignItems: 'center',
        justifyContent: 'center',
        elevation: 4,
      },
      forgot_button: {
        marginTop: 15,
        marginBottom: 30
      },
  })