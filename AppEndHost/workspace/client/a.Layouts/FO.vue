<template>
    <div class="d-flex flex-column">
        <nav class="navbar navbar-expand-lg navbar-custom fixed-top">
        <div class="container">
                <a class="navbar-brand fw-bold d-flex align-items-center" :href="getAppBasePath()">
                    <img :src="getLogoPath()" alt="AppEnd" style="width: 35px; height: 35px;" class="d-inline-block ms-2" />
                    <span>AppEnd</span>
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
            <div class="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                        <li class="nav-item">
                            <a class="nav-link" href="#features-section" @click.prevent="scrollTo('features-section')">
                                {{ translate("Features") }}
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#products-section" @click.prevent="scrollTo('products-section')">
                                {{ translate("Products") }}
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#about-section" @click.prevent="scrollTo('about-section')">
                                {{ translate("About Us") }}
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#pricing-section" @click.prevent="scrollTo('pricing-section')">
                                {{ translate("Pricing") }}
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#contact-section" @click.prevent="scrollTo('contact-section')">
                                {{ translate("Contact Us") }}
                            </a>
                        </li>
                </ul>
                    <div class="d-flex gap-2">
                        <button class="btn btn-primary-custom text-white" @click="goToLogin">
                            <i class="fa-solid fa-fw fa-sign-in-alt me-2"></i>
                            {{ translate("Login") }}
                        </button>
                    </div>
            </div>
        </div>
    </nav>
        <main style="margin-top: 76px;">
            <component-loader :src="getComponentSrc()" cid="dynamicContent" />
        </main>
    </div>
</template>

<script>
let _this = { cid: "", c: null };

export default {
    setup(props) { 
        _this.cid = props['cid']; 
    },
    data() { 
        return _this; 
    },
    created() { 
        _this.c = this; 
    },
    mounted() {
        // Update navbar on scroll
        window.addEventListener('scroll', this.handleScroll);
    },
    beforeUnmount() {
        window.removeEventListener('scroll', this.handleScroll);
    },
    methods: {
        getComponentSrc() {
            try {
                const queryComp = typeof getQueryString === 'function' ? getQueryString('c') : null;
                if (queryComp) {
                    return queryComp;
                }
                // Try to get defaultComponent from app.json
                if (typeof shared !== 'undefined' && typeof shared.getAppConfig === 'function') {
                    try {
                        const appConfig = shared.getAppConfig();
                        if (appConfig && appConfig.defaultComponent) {
                            return appConfig.defaultComponent;
                        }
                    } catch (e) {
                        console.warn('Could not get app config:', e);
                    }
                }
                // Fallback to default
                return 'components/Home';
            } catch (e) {
                console.error('Error in getComponentSrc:', e);
                return 'components/Home';
            }
        },
        translate(key) {
            if (typeof shared !== 'undefined' && typeof shared.translate === 'function') {
                return shared.translate(key);
            }
            return key;
        },
        scrollTo(sectionId) {
            const section = document.querySelector(`#${sectionId}`);
            if (section) {
                const offset = 80; // Account for fixed navbar
                const elementPosition = section.getBoundingClientRect().top;
                const offsetPosition = elementPosition + window.pageYOffset - offset;
                window.scrollTo({
                    top: offsetPosition,
                    behavior: 'smooth'
                });
            }
        },
        goToLogin() {
            window.location.href = "/AppEndStudio/";
        },
        getAppBasePath() {
            try {
                const path = window.location.pathname;
                // Extract the app folder name (e.g., /FoRtl/ or /FoLtr/)
                const match = path.match(/^\/([^\/]+)/);
                if (match && match[1]) {
                    return `/${match[1]}/`;
                }
                // Fallback to current path
                return path.endsWith('/') ? path : path + '/';
            } catch (e) {
                console.warn('Error getting app base path:', e);
                return '/';
            }
        },
        getLogoPath() {
            try {
                const basePath = this.getAppBasePath();
                return `${basePath}assets/Logo-Only.png`;
            } catch (e) {
                console.warn('Error getting logo path:', e);
                return '/assets/Logo-Only.png';
            }
        },
        handleScroll() {
            const navbar = document.querySelector('.navbar-custom');
            if (window.scrollY > 50) {
                navbar?.classList.add('shadow-sm');
            } else {
                navbar?.classList.remove('shadow-sm');
            }
        }
    },
    props: { cid: String }
}
</script>

<style scoped>
.navbar-custom {
    transition: all 0.3s ease;
}

.navbar-custom .navbar-brand img {
    transition: transform 0.3s ease;
}

.navbar-custom .navbar-brand:hover img {
    transform: rotate(5deg) scale(1.1);
}
</style>
