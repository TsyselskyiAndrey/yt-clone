import { createContext } from 'react';
import { useState } from 'react';

export const FormControlsLogInContext = createContext({});

export function FormControlsLogInProvider({children}){
    const [formControls, setFormControls] = useState({
        email: {
            type: 'email',
            name: 'email',
            label: 'Email:',
            errorMessage: '',
            value: '',
            valid: false,
            validation: {
                required: true,
                email: true,
                allowSpaces: false
            },
            touched: false,
            shake: false
        },
        password: {
            type: 'password',
            name: 'password',
            label: 'Password:',
            errorMessage: '',
            value: '',
            valid: false,
            validation: {
                required: true,
                requireNums: true,
                requireBothCases: true,
                allowSpaces: false,
                allowSymbols: false,
                minLength: 12,
                maxLength: 100
            },
            touched: false,
            shake: false
        }
    });
    return (
        <FormControlsLogInContext.Provider value={{formControls, setFormControls}}>
            {children}
        </FormControlsLogInContext.Provider>
    );
}