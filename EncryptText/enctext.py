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

# פונקציה להצפנת טקסט
def encrypt_text(text: str, password: str) -> str:
    salt = os.urandom(16)  # יצירת salt אקראי
    key = generate_key_from_password(password, salt)
    iv = os.urandom(12)  # יצירת IV אקראי
    cipher = Cipher(algorithms.AES(key), modes.GCM(iv), backend=default_backend())
    encryptor = cipher.encryptor()

    encrypted_text = encryptor.update(text.encode()) + encryptor.finalize()
    # החזרת הנתונים המוצפנים עם ה-salt, iv וה-tag בתצורת base64
    return base64.b64encode(salt + iv + encryptor.tag + encrypted_text).decode()

# פונקציה לפענוח טקסט


# # דוגמה לשימוש
# # password = "my_secret_password"  # מפתח טקסט אישי

# password = "this_is_sec"
# text_to_encrypt = "david"  # טקסט להצפנה

# # הצפנה
# encrypted_text = encrypt_text(text_to_encrypt, password)
# print(f"Encrypted text: {encrypted_text}")



