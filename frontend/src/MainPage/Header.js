import { cities } from './Cities';
import Select from 'react-select';
import React, { useState } from 'react';

function Header() {
    const [selectedCity, setSelectedCity] = useState(null);
    const handleChange = (selectedOption) => {
        setSelectedCity(selectedOption);
    };
    return (
        <div className="dropdown-container">
            Header
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
    )
}

export default Header;
