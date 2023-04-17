import React from "react";
import { View, Text } from "react-native";
import { useTheme } from "react-native-paper";

export default function HomeScreen() {
    const theme = useTheme();

    return (
        <View style={{ backgroundColor: theme.colors.primary }}>
            <Text>Home Screen</Text>
        </View>
    );
}
