import React, { useState } from "react";
import { View, Alert, StyleSheet } from "react-native";
import { supabase } from "../utils/initSupabase";
import { Avatar, Button, TextInput } from "react-native-paper";

export default function SignUpScreen() {
    const [email, setEmail] = useState('');
    const [firstPassword, setFirstPassword] = useState('');
    const [secondPassword, setSecondPassword] = useState('');
    const [loading, setLoading] = useState(false);
    const [secureFirstPasswordEntry, setFirstSecurePasswordEntry] = useState(true);
    const [secureSecondPasswordEntry, setSecondSecurePasswordEntry] = useState(true);

    async function signUpWithEmail() {
        if(email === '') {
            Alert.alert("Email is required");
            return;
        }

        if(firstPassword !== secondPassword) {
            Alert.alert("Passwords do not match");
            return;
        }

        setLoading(true);
        const { error } = await supabase.auth.signUp({
          email: email,
          password: firstPassword,
        })
    
        if (error) {
            Alert.alert(error.message);
        } else {
            Alert.alert('Success', 'Your account has been created. Please check your email for a confirmation link.');
        }

        setLoading(false);
    }
      
    return (
        <View style={styles.container}>
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
                    onChangeText={(text) => setFirstPassword(text)}
                    value={firstPassword}
                    secureTextEntry={secureFirstPasswordEntry}
                    placeholder="Password"
                    autoCapitalize={'none'}
                    right={
                        <TextInput.Icon
                          icon={secureFirstPasswordEntry ? 'eye' : 'eye-off'}
                          onPress={() => setFirstSecurePasswordEntry(!secureFirstPasswordEntry)} />
                          }
                />
            </View>
            <View style={styles.verticallySpaced}>
                <TextInput
                    label="Re-Enter Password"
                    style={styles.textInput}
                    onChangeText={(text) => setSecondPassword(text)}
                    value={secondPassword}
                    secureTextEntry={secureSecondPasswordEntry}
                    placeholder="Re-Enter Password"
                    autoCapitalize={'none'}
                    right={
                        <TextInput.Icon
                          icon={secureSecondPasswordEntry ? 'eye' : 'eye-off'}
                          onPress={() => setSecondSecurePasswordEntry(!secureSecondPasswordEntry)} />
                          }
                />
            </View>
            <View style={[styles.verticallySpaced, styles.mt20]}>
                <Button 
                    disabled={loading} 
                    onPress={() => signUpWithEmail()} 
                    mode="contained-tonal"
                    style={styles.button}>
                        Sign Up
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