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
            <div className={styles.list} >
                <label className={styles.label} >
                    <img width='20rem' className={styles.favourites}
                        src={'/img/heart-unliked.svg'}
                    ></img>
                    Обране
                </label>
            </div>


        </div>
    )
}

export default Header;
