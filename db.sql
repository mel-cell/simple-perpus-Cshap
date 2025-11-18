-- Dibuat oleh Gemini berdasarkan ERD
-- Skrip ini dirancang untuk PostgreSQL

-- Langkah 1: Buat tipe data kustom untuk 'user_level'
-- PostgreSQL menggunakan tipe ENUM kustom, yang lebih baik daripada CHECK constraint
CREATE TYPE user_level_enum AS ENUM ('admin', 'anggota');

-- Langkah 2: Buat tabel yang tidak memiliki dependensi (foreign key)

CREATE TABLE users (
    user_id VARCHAR(50) PRIMARY KEY NOT NULL,
    user_name VARCHAR(50) NOT NULL,
    user_alamat VARCHAR(50) NOT NULL,
    -- Disarankan menambahkan UNIQUE constraint pada username dan email
    user_username VARCHAR(50) NOT NULL UNIQUE,
    user_email VARCHAR(50) NOT NULL UNIQUE,
    user_notelp CHAR(13) NOT NULL,
    -- PERHATIAN: Menyimpan password sebagai VARCHAR(50) tidak aman.
    -- Sebaiknya gunakan VARCHAR(255) atau TEXT untuk menyimpan hash password.
    user_password VARCHAR(50) NOT NULL,
    user_level user_level_enum NOT NULL
);

CREATE TABLE penulis (
    penulis_id VARCHAR(50) PRIMARY KEY NOT NULL,
    penulis_nama VARCHAR(50) NOT NULL,
    penulis_tmptlahir VARCHAR(50) NOT NULL,
    penulis_tgllahir DATE NOT NULL
);

CREATE TABLE kategori (
    kategori_id VARCHAR(50) PRIMARY KEY NOT NULL,
    kategori_nama VARCHAR(20) NOT NULL
);

CREATE TABLE penerbit (
    penerbit_id VARCHAR(50) PRIMARY KEY NOT NULL,
    penerbit_nama VARCHAR(50) NOT NULL,
    penerbit_alamat VARCHAR(50) NOT NULL,
    penerbit_notelp CHAR(13) NOT NULL,
    penerbit_email VARCHAR(50) NOT NULL
);

CREATE TABLE rak (
    rak_id VARCHAR(50) PRIMARY KEY NOT NULL,
    rak_nama VARCHAR(20) NOT NULL,
    rak_lokasi VARCHAR(50) NOT NULL,
    rak_kapasitas INT NOT NULL
);

-- Langkah 3: Buat tabel yang memiliki dependensi

CREATE TABLE buku (
    buku_id VARCHAR(50) PRIMARY KEY NOT NULL,
    buku_penulis_id VARCHAR(50) NOT NULL,
    buku_kategori_id VARCHAR(50) NOT NULL,
    buku_penerbit_id VARCHAR(50) NOT NULL,
    buku_rak_id VARCHAR(50) NOT NULL,
    buku_judul VARCHAR(40) NOT NULL,
    -- Catatan: ISBN modern biasanya 13 karakter (CHAR(13))
    buku_isbn CHAR(10) NOT NULL,
    buku_thnterbit CHAR(4) NOT NULL,
    buku_stok INT NOT NULL DEFAULT 0,

    -- Mendefinisikan Foreign Keys
    CONSTRAINT fk_penulis FOREIGN KEY (buku_penulis_id) REFERENCES penulis(penulis_id),
    CONSTRAINT fk_kategori FOREIGN KEY (buku_kategori_id) REFERENCES kategori(kategori_id),
    CONSTRAINT fk_penerbit FOREIGN KEY (buku_penerbit_id) REFERENCES penerbit(penerbit_id),
    CONSTRAINT fk_rak FOREIGN KEY (buku_rak_id) REFERENCES rak(rak_id)
);

CREATE TABLE peminjaman (
    peminjaman_id VARCHAR(50) PRIMARY KEY NOT NULL,
    peminjaman_user_id VARCHAR(50) NOT NULL,
    peminjaman_tglpinjam DATE NOT NULL,
    peminjaman_tglkembali DATE NOT NULL,
    -- DEFAULT FALSE sudah sesuai dengan diagram
    peminjaman_statuskembali BOOLEAN DEFAULT FALSE,
    -- Kolom tanpa NOT NULL otomatis memperbolehkan NULL
    peminjaman_note VARCHAR(100),
    peminjaman_denda INT,
    
    -- Mendefinisikan Foreign Key
    CONSTRAINT fk_users FOREIGN KEY (peminjaman_user_id) REFERENCES users(user_id)
);

-- Langkah 4: Buat tabel junction/detail (dependensi terakhir)

CREATE TABLE peminjaman_detail (
    peminjaman_detail_id VARCHAR(50) PRIMARY KEY NOT NULL,
    peminjaman_detail_peminjaman_id VARCHAR(50) NOT NULL,
    peminjaman_detail_buku_id VARCHAR(50) NOT NULL,
    
    -- Mendefinisikan Foreign Keys
    CONSTRAINT fk_peminjaman FOREIGN KEY (peminjaman_detail_peminjaman_id) REFERENCES peminjaman(peminjaman_id),
    CONSTRAINT fk_buku FOREIGN KEY (peminjaman_detail_buku_id) REFERENCES buku(buku_id)
);
