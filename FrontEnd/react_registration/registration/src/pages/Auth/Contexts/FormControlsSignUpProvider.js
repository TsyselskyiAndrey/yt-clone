import { createContext } from 'react';
import { useState } from 'react';

export const FormControlsSignUpContext = createContext({});

export function FormControlsSignUpProvider({ children }) {
    const [formControls, setFormControls] = useState([
        {
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
                    maxLength: 30
                },
                touched: false,
                shake: false
            },
            repassword: {
                type: 'password',
                name: 're-password',
                label: 'Re-Password:',
                errorMessage: '',
                value: '',
                valid: false,
                validation: {
                    required: true,
                    repassword: true
                },
                touched: false,
                shake: false
            }
        },
        {
            firstname: {
                type: 'text',
                name: 'name',
                label: 'First Name:',
                errorMessage: '',
                value: '',
                valid: false,
                validation: {
                    required: true,
                    allowSpaces: false,
                    allowSymbols: false,
                    allowNums: false,
                    minLength: 2,
                    maxLength: 20
                },
                touched: false,
                shake: false
            },
            lastname: {
                type: 'text',
                name: 'surname',
                label: 'Last Name:',
                errorMessage: '',
                value: '',
                valid: false,
                validation: {
                    required: true,
                    allowSpaces: false,
                    allowSymbols: false,
                    allowNums: false,
                    minLength: 2,
                    maxLength: 20
                },
                touched: false,
                shake: false
            },
            birthdate: {
                type: 'date',
                name: 'birthdate',
                label: 'Birth date (MM/DD/YYYY):',
                errorMessage: '',
                value: '',
                valid: false,
                validation: {
                    required: true,
                    date: true
                },
                touched: false,
                shake: false
            }
        },
        {
            code: {
                type: 'code',
                name: 'code',
                label: '',
                errorMessage: '',
                value: "",
                valid: false,
                validation: {
                    code: true
                },
                touched: false,
                shake: false
            }
        }
    ]);
    return (
        <FormControlsSignUpContext.Provider value={{formControls, setFormControls}}>
            {children}
        </FormControlsSignUpContext.Provider>
    );
}
