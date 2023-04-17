import React from "react";
import { View, Text, StyleSheet } from "react-native";
import { Button } from "react-native-paper";
import { supabase } from "../utils/initSupabase";

export default function SettingsScreen() {
    async function signOut() {
        supabase.auth.signOut();
      }

    return (
        <View>
            <View><Text>Settings Screen</Text></View>
            <View style={[styles.verticallySpaced, styles.mt20]}>
                    <Button onPress={() => signOut()}>Sign Out</Button>
            </View>
        </View>
    );
}

const styles = StyleSheet.create({
    container: {
      marginTop: 40,
      padding: 12,
    },
    verticallySpaced: {
      paddingTop: 4,
      paddingBottom: 4,
      alignSelf: 'stretch',
    },
    mt20: {
      marginTop: 20,
    },
  })