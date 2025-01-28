import { createContext } from 'react';
import { useMediaQuery } from '@mui/material';

export const IsSmallContext = createContext(false);

export function IsSmallProvider({ children }) {
    const isSmall = useMediaQuery('(max-width: 1300px) or (max-height: 600px) or (max-aspect-ratio: 13/9)');
    return (
        <IsSmallContext.Provider value={{isSmall}}>
            {children}
        </IsSmallContext.Provider>
    );
}