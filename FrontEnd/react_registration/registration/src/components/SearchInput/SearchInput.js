import { useState, useId } from 'react';
import './SearchInput.css';
import magnifier1 from './resources/magnifier1.png'
import magnifier2 from './resources/magnifier2.png'
export default function SearchInput(props) {
    const id = useId();
    return (
        <>
            <div className='searchBox'>
                <input id={id} type="text" placeholder={props.label} value={props.searchValue} onChange={(e) => props.setSearchValue(e.target.value)} autoComplete='off'/>
                <img className='magnifier' src={magnifier1}></img>
                <button className='searchBtn' onClick={props.handleSearch}><img src={magnifier2}></img>
                    <div className="tooltip">Search</div>
                </button>
            </div>

        </>
    );
}

