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
            Header
            <div className={styles.select}>
                <Select
                    value={selectedCity}
                    onChange={handleChange}
                    options={cities}
                    placeholder="Місто"
                    className="dropdown"
                    classNamePrefix="dropdown"
                    isClearable={true}
                    isSearchable={true}
                />
            </div>
        </div>
    )
}

export default Header;
