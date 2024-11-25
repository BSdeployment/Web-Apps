**Project: AES Encryption and Decryption using Flask and Blazor WASM**
====================================================================

### Introduction

This project demonstrates the implementation of AES encryption and decryption using Flask as the backend and Blazor WASM as the frontend.

### Project Structure

```
Project
|____ Msuka (Flask backend)
|       |____ app.py
|       |____ templates
|       |       |____ index.html
|____ EncryptDecryptWASM (Blazor WASM frontend)
|       |____ Program.cs
|       |____ NavigationMenu.razor
|       |____ Index.razor
|____ test
|       |____ test_aes.py
|____ requirements.txt
```

### Backend (Flask)

The Flask backend provides two APIs:

*   **Encryption API:** Accepts a string as input, encrypts it using AES, and returns the encrypted string.
*   **Decryption API:** Accepts an encrypted string as input, decrypts it using AES, and returns the original string.

The APIs use the `cryptography` library to implement AES encryption and decryption.

### Frontend (Blazor WASM)

The Blazor WASM frontend provides a simple user interface for users to input and encrypt data, as well as decrypt encrypted data.

### AES Algorithm

The AES algorithm used in this project is AES-256-CBC. The secret key is 256-bit and the initialization vector (IV) is 128-bit.




### Running the Project

To run the project, follow these steps:

1.  Clone the repository: `git clone https://github.com/your-username/aes-encryption-decryption.git`
2.  Install dependencies: `pip install -r requirements.txt`
3.  Run the Flask backend: `python app.py`
4.  Run the Blazor WASM frontend: `dotnet run`

Note: Replace `secret_key_here` and `initialization_vector_here` with your actual secret key and initialization vector.

## example
<img src=""/>
