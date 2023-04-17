import { cities } from './Cities';
import Select from 'react-select';
import React, { useState } from 'react';
import styles from './Header.module.scss';

function Header() {
    const [selectedCity, setSelectedCity] = useState(null);
    const handleChange = (selectedOption) => {
        console.log(selectedOption);
        setSelectedCity(selectedOption);
    };
    return (
        <div className={styles.headerMain}>
            <div className={styles.select}>
                
            </div>
        </div>
    )
}

export default Header;
