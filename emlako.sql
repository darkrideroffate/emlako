--
-- PostgreSQL database dump
--

-- Dumped from database version 13.1
-- Dumped by pg_dump version 13.1

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: emlako; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE emlako WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'English_United States.1252';


ALTER DATABASE emlako OWNER TO postgres;

\connect emlako

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: returntype; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.returntype AS (
	id integer,
	ev_sahibi_id integer,
	name character varying,
	ev_tipi integer,
	adres_id integer,
	kira_fiyati integer,
	il character varying,
	ilce character varying,
	satir1 character varying
);


ALTER TYPE public.returntype OWNER TO postgres;

--
-- Name: ad_soyad_birlestir(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.ad_soyad_birlestir() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
  new."AdSoyad" = concat(new.ad,' ',new.soyad);
  RETURN NEW;
END;
$$;


ALTER FUNCTION public.ad_soyad_birlestir() OWNER TO postgres;

--
-- Name: discount_hesapla(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.discount_hesapla() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
  NEW."discountedPrice" = (100 - NEW.discount )::float/ 100 * NEW.kira_fiyati;
  RETURN NEW;
END;
$$;


ALTER FUNCTION public.discount_hesapla() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: ev; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ev (
    id integer NOT NULL,
    ev_sahibi_id integer NOT NULL,
    name character varying(2044) NOT NULL,
    ev_tipi character varying(2044) NOT NULL,
    kira_fiyati integer NOT NULL,
    adres_id integer NOT NULL,
    discount integer DEFAULT 0 NOT NULL,
    "discountedPrice" integer DEFAULT 0 NOT NULL
);


ALTER TABLE public.ev OWNER TO postgres;

--
-- Name: get_ev_by_ilce(character varying); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_ev_by_ilce(_ilce character varying) RETURNS SETOF public.ev
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY SELECT *
                   FROM ev
                  WHERE (select adres.ilce from adres where adres.id = ev.adres_id ) like '%' || _ilce || '%';

    IF NOT FOUND THEN
        RAISE EXCEPTION '% ilceli bir ev yok.', _ilce;
    END IF;

    RETURN;
 END;
$$;


ALTER FUNCTION public.get_ev_by_ilce(_ilce character varying) OWNER TO postgres;

--
-- Name: get_ev_under(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_ev_under(i integer) RETURNS SETOF public.ev
    LANGUAGE plpgsql
    AS $_$
BEGIN
    RETURN QUERY SELECT *
                   FROM ev
                  WHERE ev.kira_fiyati <= $1;

    IF NOT FOUND THEN
        RAISE EXCEPTION '% altinda bir ev yok.', $1;
    END IF;

    RETURN;
 END;
$_$;


ALTER FUNCTION public.get_ev_under(i integer) OWNER TO postgres;

--
-- Name: get_gunluk_kira(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_gunluk_kira(ev_id integer) RETURNS integer
    LANGUAGE plpgsql
    AS $_$

declare gunluk_kira int;
BEGIN
    
    gunluk_kira := ((SELECT kira_fiyati 
                   FROM ev
                  WHERE ev.id = ev_id) / 30)::float;
    

--     IF NOT FOUND THEN
--         RAISE EXCEPTION '% idli bir ev yok.', $1;
--     END IF;

    RETURN  gunluk_kira ;
 END;
$_$;


ALTER FUNCTION public.get_gunluk_kira(ev_id integer) OWNER TO postgres;

--
-- Name: get_yillik_kira(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_yillik_kira(ev_id integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$

declare yillik_kira int;
BEGIN
    
    yillik_kira := (SELECT kira_fiyati 
                   FROM ev
                  WHERE ev.id = ev_id) * 12;
    


    RETURN  yillik_kira ;
 END;
$$;


ALTER FUNCTION public.get_yillik_kira(ev_id integer) OWNER TO postgres;

--
-- Name: trigger_set_timestamp(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.trigger_set_timestamp() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
  NEW.updated_at = NOW();
  RETURN NEW;
END;
$$;


ALTER FUNCTION public.trigger_set_timestamp() OWNER TO postgres;

--
-- Name: user_insert_trigger_fnc(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.user_insert_trigger_fnc() RETURNS trigger
    LANGUAGE plpgsql
    AS $$

BEGIN



    INSERT INTO "User_Audit" ( "id", "soyad", "ad" ,"usertime")

         VALUES(NEW."id",NEW."soyad",NEW."ad",current_date);



RETURN NEW;

END;

$$;


ALTER FUNCTION public.user_insert_trigger_fnc() OWNER TO postgres;

--
-- Name: User_Audit; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."User_Audit" (
    id integer NOT NULL,
    ad character varying NOT NULL,
    soyad character varying NOT NULL,
    usertime timestamp with time zone NOT NULL
);


ALTER TABLE public."User_Audit" OWNER TO postgres;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Name: admin; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.admin (
    id integer NOT NULL
);


ALTER TABLE public.admin OWNER TO postgres;

--
-- Name: adres; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.adres (
    id integer NOT NULL,
    ilce character varying(2044) NOT NULL,
    il character varying(2044) NOT NULL,
    satir1 character varying(2044) NOT NULL
);


ALTER TABLE public.adres OWNER TO postgres;

--
-- Name: adres_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.adres_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.adres_id_seq OWNER TO postgres;

--
-- Name: adres_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.adres_id_seq OWNED BY public.adres.id;


--
-- Name: esya; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.esya (
    id integer NOT NULL,
    ev_id integer NOT NULL,
    esya_tipi character varying(2044) NOT NULL
);


ALTER TABLE public.esya OWNER TO postgres;

--
-- Name: esya_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.esya_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.esya_id_seq OWNER TO postgres;

--
-- Name: esya_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.esya_id_seq OWNED BY public.esya.id;


--
-- Name: ev_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.ev_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.ev_id_seq OWNER TO postgres;

--
-- Name: ev_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.ev_id_seq OWNED BY public.ev.id;


--
-- Name: ev_kira; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ev_kira (
    id integer NOT NULL,
    personel_id integer NOT NULL,
    ev_id integer NOT NULL,
    kiraci_id integer NOT NULL,
    kira_fiyati integer NOT NULL,
    sure character varying(2044) NOT NULL
);


ALTER TABLE public.ev_kira OWNER TO postgres;

--
-- Name: ev_kira_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.ev_kira_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.ev_kira_id_seq OWNER TO postgres;

--
-- Name: ev_kira_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.ev_kira_id_seq OWNED BY public.ev_kira.id;


--
-- Name: ev_sahibi; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ev_sahibi (
    id integer NOT NULL,
    ad character varying(2044) NOT NULL,
    soyad character varying(2044) NOT NULL,
    telefon character varying(2044) NOT NULL
);


ALTER TABLE public.ev_sahibi OWNER TO postgres;

--
-- Name: ev_sahibi_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.ev_sahibi_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.ev_sahibi_id_seq OWNER TO postgres;

--
-- Name: ev_sahibi_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.ev_sahibi_id_seq OWNED BY public.ev_sahibi.id;


--
-- Name: fotograf; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fotograf (
    id integer NOT NULL,
    ev_id integer NOT NULL,
    file character varying(2044) NOT NULL
);


ALTER TABLE public.fotograf OWNER TO postgres;

--
-- Name: fotograf_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.fotograf_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.fotograf_id_seq OWNER TO postgres;

--
-- Name: fotograf_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.fotograf_id_seq OWNED BY public.fotograf.id;


--
-- Name: ilan_koy; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ilan_koy (
    id integer NOT NULL,
    ev_id integer NOT NULL,
    ofis_id integer NOT NULL
);


ALTER TABLE public.ilan_koy OWNER TO postgres;

--
-- Name: ilan_koy_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.ilan_koy_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.ilan_koy_id_seq OWNER TO postgres;

--
-- Name: ilan_koy_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.ilan_koy_id_seq OWNED BY public.ilan_koy.id;


--
-- Name: kiraci; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.kiraci (
    id integer NOT NULL,
    ad character varying(2044) NOT NULL,
    soyad character varying(2044) NOT NULL
);


ALTER TABLE public.kiraci OWNER TO postgres;

--
-- Name: kiraci_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.kiraci_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.kiraci_id_seq OWNER TO postgres;

--
-- Name: kiraci_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.kiraci_id_seq OWNED BY public.kiraci.id;


--
-- Name: oda; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.oda (
    id integer NOT NULL,
    ev_id integer NOT NULL,
    oda_tipi character varying(2044) NOT NULL
);


ALTER TABLE public.oda OWNER TO postgres;

--
-- Name: oda_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.oda_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.oda_id_seq OWNER TO postgres;

--
-- Name: oda_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.oda_id_seq OWNED BY public.oda.id;


--
-- Name: ofis; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ofis (
    id integer NOT NULL,
    name character varying(2044) NOT NULL
);


ALTER TABLE public.ofis OWNER TO postgres;

--
-- Name: ofis_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.ofis_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.ofis_id_seq OWNER TO postgres;

--
-- Name: ofis_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.ofis_id_seq OWNED BY public.ofis.id;


--
-- Name: ozellik; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ozellik (
    id integer NOT NULL,
    ev_id integer NOT NULL,
    ozellik_tipi character varying(2044) NOT NULL
);


ALTER TABLE public.ozellik OWNER TO postgres;

--
-- Name: ozellik_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.ozellik_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.ozellik_id_seq OWNER TO postgres;

--
-- Name: ozellik_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.ozellik_id_seq OWNED BY public.ozellik.id;


--
-- Name: personel; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.personel (
    id integer NOT NULL,
    ofis_id integer NOT NULL
);


ALTER TABLE public.personel OWNER TO postgres;

--
-- Name: personel_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.personel_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.personel_id_seq OWNER TO postgres;

--
-- Name: personel_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.personel_id_seq OWNED BY public.personel.id;


--
-- Name: user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."user" (
    id integer NOT NULL,
    ad character varying(2044) NOT NULL,
    soyad character varying(2044) NOT NULL,
    email character varying(2044) NOT NULL,
    hash character varying(2044) NOT NULL,
    kisi_turu character varying(2044) NOT NULL,
    salt character varying(2044) NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL,
    updated_at timestamp with time zone DEFAULT now() NOT NULL,
    "AdSoyad" text
);


ALTER TABLE public."user" OWNER TO postgres;

--
-- Name: user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.user_id_seq OWNER TO postgres;

--
-- Name: user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.user_id_seq OWNED BY public."user".id;


--
-- Name: adres id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.adres ALTER COLUMN id SET DEFAULT nextval('public.adres_id_seq'::regclass);


--
-- Name: esya id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.esya ALTER COLUMN id SET DEFAULT nextval('public.esya_id_seq'::regclass);


--
-- Name: ev id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ev ALTER COLUMN id SET DEFAULT nextval('public.ev_id_seq'::regclass);


--
-- Name: ev_kira id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ev_kira ALTER COLUMN id SET DEFAULT nextval('public.ev_kira_id_seq'::regclass);


--
-- Name: ev_sahibi id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ev_sahibi ALTER COLUMN id SET DEFAULT nextval('public.ev_sahibi_id_seq'::regclass);


--
-- Name: fotograf id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.fotograf ALTER COLUMN id SET DEFAULT nextval('public.fotograf_id_seq'::regclass);


--
-- Name: ilan_koy id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ilan_koy ALTER COLUMN id SET DEFAULT nextval('public.ilan_koy_id_seq'::regclass);


--
-- Name: kiraci id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kiraci ALTER COLUMN id SET DEFAULT nextval('public.kiraci_id_seq'::regclass);


--
-- Name: oda id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda ALTER COLUMN id SET DEFAULT nextval('public.oda_id_seq'::regclass);


--
-- Name: ofis id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ofis ALTER COLUMN id SET DEFAULT nextval('public.ofis_id_seq'::regclass);


--
-- Name: ozellik id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ozellik ALTER COLUMN id SET DEFAULT nextval('public.ozellik_id_seq'::regclass);


--
-- Name: user id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user" ALTER COLUMN id SET DEFAULT nextval('public.user_id_seq'::regclass);


--
-- Data for Name: User_Audit; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."User_Audit" VALUES
	(3, '1234', '123', '2020-12-30 00:00:00+03'),
	(4, 'khaled', 'elnasan', '2020-12-30 00:00:00+03'),
	(5, 'ahmet', 'musa', '2020-12-30 00:00:00+03');


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."__EFMigrationsHistory" VALUES
	('20201230100222_empting', '3.1.10'),
	('20201230112543_removed-ev-id-from-adres', '3.1.10'),
	('20201230163313_discount', '3.1.10'),
	('20201230173822_fullname', '3.1.10');


--
-- Data for Name: admin; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: adres; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.adres VALUES
	(1, 'istanbul', 'istanbul', 'oge sokak daire 1');


--
-- Data for Name: esya; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.esya VALUES
	(1, 1, 'Buzdulabi'),
	(2, 3, 'Camasir Makinesi');


--
-- Data for Name: ev; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.ev VALUES
	(3, 1, 'guzel', 'ev', 2000, 1, 30, 1400),
	(4, 1, 'sdf', 'sd', 1222, 1, 50, 611),
	(1, 1, 'Guzel ev', 'Apart', 1000, 1, 50, 500);


--
-- Data for Name: ev_kira; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.ev_kira VALUES
	(1, 1, 3, 1, 1000, '6');


--
-- Data for Name: ev_sahibi; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.ev_sahibi VALUES
	(1, 'Abdullah', 'Jamous', '+905078432805');


--
-- Data for Name: fotograf; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.fotograf VALUES
	(2, 1, '8f1fcc47-6446-4974-9af5-e7e99a33a691_Logo-Arabic.png'),
	(3, 1, '8cb2fa36-9134-4437-8422-b430ed17ebca_Logo.png');


--
-- Data for Name: ilan_koy; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- Data for Name: kiraci; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.kiraci VALUES
	(1, 'Mahmt', 'Ali');


--
-- Data for Name: oda; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.oda VALUES
	(1, 1, 'tuvalet'),
	(2, 1, 'buyuk'),
	(4, 3, 'yatak odasi');


--
-- Data for Name: ofis; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.ofis VALUES
	(1, 'emlako'),
	(2, 'gayrimenkolo');


--
-- Data for Name: ozellik; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.ozellik VALUES
	(1, 1, 'elektrik'),
	(2, 3, 'elektrik'),
	(3, 3, 'gaz'),
	(4, 3, 'su');


--
-- Data for Name: personel; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.personel VALUES
	(1, 1);


--
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."user" VALUES
	(1, 'halid', 'elnasann', 'halid@123,com', '1234', '1', '1234', '2020-12-24 15:58:47.40929+03', '2020-12-30 21:02:31.947596+03', 'halid elnasann'),
	(4, 'khaled', 'elnasan', 'kha@123.com', '123', '2', '123', '2020-12-30 21:58:21.601756+03', '2020-12-30 21:58:21.601756+03', 'khaled elnasan'),
	(5, 'ahmet', 'musa', 'ah@123.com', '123', '2', '123', '2020-12-30 21:58:37.421826+03', '2020-12-30 21:58:37.421826+03', 'ahmet musa');


--
-- Name: adres_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.adres_id_seq', 1, true);


--
-- Name: esya_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.esya_id_seq', 2, true);


--
-- Name: ev_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.ev_id_seq', 2, false);


--
-- Name: ev_kira_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.ev_kira_id_seq', 1, true);


--
-- Name: ev_sahibi_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.ev_sahibi_id_seq', 1, true);


--
-- Name: fotograf_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.fotograf_id_seq', 3, true);


--
-- Name: ilan_koy_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.ilan_koy_id_seq', 1, false);


--
-- Name: kiraci_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.kiraci_id_seq', 1, true);


--
-- Name: oda_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.oda_id_seq', 4, true);


--
-- Name: ofis_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.ofis_id_seq', 2, true);


--
-- Name: ozellik_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.ozellik_id_seq', 4, true);


--
-- Name: personel_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.personel_id_seq', 1, false);


--
-- Name: user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.user_id_seq', 5, true);


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: ilan_koy ilan_koy_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ilan_koy
    ADD CONSTRAINT ilan_koy_pkey PRIMARY KEY (id);


--
-- Name: ofis ofis_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ofis
    ADD CONSTRAINT ofis_pkey PRIMARY KEY (id);


--
-- Name: personel personel_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.personel
    ADD CONSTRAINT personel_pkey PRIMARY KEY (id);


--
-- Name: admin unique_admin_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin
    ADD CONSTRAINT unique_admin_id PRIMARY KEY (id);


--
-- Name: adres unique_adres_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.adres
    ADD CONSTRAINT unique_adres_id PRIMARY KEY (id);


--
-- Name: esya unique_esya_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.esya
    ADD CONSTRAINT unique_esya_id PRIMARY KEY (id);


--
-- Name: ev unique_ev_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ev
    ADD CONSTRAINT unique_ev_id PRIMARY KEY (id);


--
-- Name: ev_kira unique_ev_kira_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ev_kira
    ADD CONSTRAINT unique_ev_kira_id PRIMARY KEY (id);


--
-- Name: ev_sahibi unique_ev_sahibi_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ev_sahibi
    ADD CONSTRAINT unique_ev_sahibi_id PRIMARY KEY (id);


--
-- Name: fotograf unique_fotograf_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.fotograf
    ADD CONSTRAINT unique_fotograf_id PRIMARY KEY (id);


--
-- Name: kiraci unique_kiraci_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.kiraci
    ADD CONSTRAINT unique_kiraci_id PRIMARY KEY (id);


--
-- Name: oda unique_oda_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda
    ADD CONSTRAINT unique_oda_id PRIMARY KEY (id);


--
-- Name: ozellik unique_ozellik_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ozellik
    ADD CONSTRAINT unique_ozellik_id PRIMARY KEY (id);


--
-- Name: user unique_user_email; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT unique_user_email UNIQUE (email);


--
-- Name: user unique_user_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT unique_user_id PRIMARY KEY (id);


--
-- Name: user ad_soyad_birle; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER ad_soyad_birle BEFORE INSERT OR UPDATE ON public."user" FOR EACH ROW EXECUTE FUNCTION public.ad_soyad_birlestir();


--
-- Name: ev discount_hesap; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER discount_hesap BEFORE INSERT OR UPDATE ON public.ev FOR EACH ROW EXECUTE FUNCTION public.discount_hesapla();


--
-- Name: user set_timestamp; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER set_timestamp BEFORE UPDATE ON public."user" FOR EACH ROW EXECUTE FUNCTION public.trigger_set_timestamp();


--
-- Name: user user_insert_trigger; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER user_insert_trigger AFTER INSERT ON public."user" FOR EACH ROW EXECUTE FUNCTION public.user_insert_trigger_fnc();


--
-- Name: ev ev_adress; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ev
    ADD CONSTRAINT ev_adress FOREIGN KEY (adres_id) REFERENCES public.adres(id) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: esya ev_esya; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.esya
    ADD CONSTRAINT ev_esya FOREIGN KEY (ev_id) REFERENCES public.ev(id) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: fotograf ev_fotograf; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.fotograf
    ADD CONSTRAINT ev_fotograf FOREIGN KEY (ev_id) REFERENCES public.ev(id) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: ilan_koy ev_ilan; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ilan_koy
    ADD CONSTRAINT ev_ilan FOREIGN KEY (ev_id) REFERENCES public.ev(id) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: ev_kira ev_kira_ev; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ev_kira
    ADD CONSTRAINT ev_kira_ev FOREIGN KEY (ev_id) REFERENCES public.ev(id) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: ev_kira ev_kira_personel; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ev_kira
    ADD CONSTRAINT ev_kira_personel FOREIGN KEY (personel_id) REFERENCES public.personel(id) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: oda ev_oda; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.oda
    ADD CONSTRAINT ev_oda FOREIGN KEY (ev_id) REFERENCES public.ev(id) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: ozellik ev_ozellik; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ozellik
    ADD CONSTRAINT ev_ozellik FOREIGN KEY (ev_id) REFERENCES public.ev(id) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: ev ev_sahibi; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ev
    ADD CONSTRAINT ev_sahibi FOREIGN KEY (ev_sahibi_id) REFERENCES public.ev_sahibi(id) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: ev_kira kiraci-evkira; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ev_kira
    ADD CONSTRAINT "kiraci-evkira" FOREIGN KEY (kiraci_id) REFERENCES public.kiraci(id) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: ilan_koy ofis_ilan; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ilan_koy
    ADD CONSTRAINT ofis_ilan FOREIGN KEY (ofis_id) REFERENCES public.ofis(id) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: personel personel_ofis; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.personel
    ADD CONSTRAINT personel_ofis FOREIGN KEY (ofis_id) REFERENCES public.ofis(id) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: personel personeluser; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.personel
    ADD CONSTRAINT personeluser FOREIGN KEY (id) REFERENCES public."user"(id) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- Name: admin useradmin; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.admin
    ADD CONSTRAINT useradmin FOREIGN KEY (id) REFERENCES public."user"(id) MATCH FULL ON UPDATE CASCADE ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

