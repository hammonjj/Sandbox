import { useCallback } from 'react';
import debounce from "lodash/debounce";
import { TextInput } from 'react-native';

interface IProps {
    callback: any;
    cssStyle?: any;
    inputId: string;
    testId?: string;
    secureEntry?: boolean;
    defaultValue?: string;
}

function DebounceTextInput(props: IProps) {
    const changeTextDebouncer = useCallback(debounce(props.callback, 500), []);
    
    return (
        <TextInput 
            style={props.cssStyle} 
            id={props.inputId} 
            onChangeText={changeTextDebouncer}
            data-testid={props.testId} 
            secureTextEntry={props.secureEntry} 
            defaultValue={props.defaultValue} />
    );
}

export default DebounceTextInput;