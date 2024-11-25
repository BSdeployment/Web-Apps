from cryptography.hazmat.primitives.ciphers import Cipher, algorithms, modes
from cryptography.hazmat.backends import default_backend
from cryptography.hazmat.primitives.kdf.pbkdf2 import PBKDF2HMAC
from cryptography.hazmat.primitives import hashes
import os
import base64

# פונקציה ליצירת מפתח על בסיס סיסמה
def generate_key_from_password(password: str, salt: bytes) -> bytes:
    kdf = PBKDF2HMAC(
        algorithm=hashes.SHA256(),
        length=32,
        salt=salt,
        iterations=100000,
        backend=default_backend()
    )
    return kdf.derive(password.encode())




# פונקציה לפענוח טקסט
def decrypt_text(encrypted_text: str, password: str) -> str:
    # פענוח המידע מ-base64 וקריאת ה-salt, iv, tag והטקסט המוצפן
    data = base64.b64decode(encrypted_text.encode())
    salt, iv, tag, encrypted_data = data[:16], data[16:28], data[28:44], data[44:]

    # יצירת מפתח מהסיסמה עם אותו salt
    key = generate_key_from_password(password, salt)
    cipher = Cipher(algorithms.AES(key), modes.GCM(iv, tag), backend=default_backend())
    decryptor = cipher.decryptor()

    myres = decryptor.update(encrypted_data) + decryptor.finalize()
    # פענוח והחזרת הטקסט המקורי
    return myres.decode()

# # דוגמה לשימוש
# password = "this_is_sec"  # מפתח טקסט אישי


# # פענוח
# decrypted_text = decrypt_text("FAVf3tVM1lZycdztybega8YWPqkvy0zbGiajPv1SWyqrEvljiP16CPN20IfWKs5SvA==", password)
# print(f"Decrypted text: {decrypted_text}")
