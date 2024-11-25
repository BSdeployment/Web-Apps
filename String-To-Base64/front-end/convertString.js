'use client'
import { useState } from 'react'
import 'bootstrap/dist/css/bootstrap.min.css'
import axios from 'axios'

export default function MyComponent() {
    const [stringToBase64, setBase64] = useState(null);
    const [base64ToString, setString] = useState(null);
    const [myString, setMyString] = useState(null);
    const [base64, setMybase] = useState(null);

    const ConverterStringToBase = async (mystring) => {
        try {
            const response = await axios.get(`https://localhost:7035/string-to-base64/${mystring}`)
            setBase64(response.data)
        } catch (error) {
            console.error('Error posting data:', error);
            alert(error);
        }
    }

    const ConverterBase64ToString = async (mybase64) => {
        try {
            const response = await axios.get(`https://localhost:7035/base64-to-string/${mybase64}`);
            setString(response.data);
        }
        catch (error) {
            console.error('Error posting data:', error);
            alert(error);
        }



    }


    return (
        <>
            <div className='card p-3 col-4 mx-auto'  >
                <div className='text-center'>
                    <h2>String To base64</h2>
                    <input type='text' className='form-control mb-3' onChange={(e) => setMyString(e.target.value)} />
                    <button className='btn btn-primary' onClick={() => ConverterStringToBase(myString)}>click</button>
                    <h3 className='text-center' >{stringToBase64}</h3>
                </div>


            </div>
            <div className='card p-3 col-4 mx-auto mt-3'  >
                <div className='text-center'>
                    <h2>base64 to string</h2>
                    <input type='text' className='form-control mb-3' onChange={(e)=>setMybase(e.target.value)} />
                    <button className='btn btn-primary' onClick={()=>ConverterBase64ToString(base64)} >click</button>
                    <h3 className='text-center'>{base64ToString}</h3>
                </div>


            </div>
        </>
    )
}