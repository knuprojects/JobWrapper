import React from 'react'
function Items({ name, skills, location, salary, id }) {
    return (
        <div key = {id}>
            <h4>{name}</h4>
            <h3>{skills.map((item) => item + ' , ')}</h3>
            <h3>{skills}</h3>
            <h3>{location}</h3>
            <h3>{salary}</h3>
        </div>
    )
}

export default Items;
