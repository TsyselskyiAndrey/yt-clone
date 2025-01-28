import { createContext } from 'react';
import { useState } from 'react';

export const IsSubmitLoadingContext = createContext({});

export function IsSubmitLoadingProvider({children}){
    const [isSubmitLoading, setIsSubmitLoading] = useState(false);
    return (
        <IsSubmitLoadingContext.Provider value={{isSubmitLoading, setIsSubmitLoading}}>
            {children}
        </IsSubmitLoadingContext.Provider>
    );
}