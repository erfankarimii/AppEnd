<template>
    <section class="hero-section">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-24 hero-content">
                    <h1 class="display-3 fw-bold mb-4 animate__animated animate__fadeInUp">
                        {{ translate("AppEnd") }} - {{ translate("Low-Code Platform") }}
                    </h1>
                    <p class="lead mb-4 animate__animated animate__fadeInUp animate__delay-1s">
                        {{ translate("A full-stack low-code platform that helps you build APIs, user interfaces, and manage access levels rapidly.") }}
                    </p>
                    <div class="d-flex gap-3 flex-wrap animate__animated animate__fadeInUp animate__delay-2s">
                        <button class="btn btn-primary-custom btn-lg text-white" @click="goToLogin">
                            <i class="fa-solid fa-fw fa-sign-in-alt me-2"></i>
                            {{ translate("Enter Dashboard") }}
                        </button>
                        <button class="btn btn-outline-light btn-lg" @click="scrollToFeatures">
                            <i class="fa-solid fa-fw fa-arrow-down me-2"></i>
                            {{ translate("Learn More") }}
                        </button>
                    </div>
                </div>
                <div class="col-24 text-center animate__animated animate__fadeInRight">
                    <div class="hero-image-placeholder">
                        <div class="hero-visual">
                            <div class="code-bracket left">{"</div>
                            <div class="code-content">
                                <div class="code-line"><span class="keyword">function</span> <span class="function">buildApp</span>() {</div>
                                <div class="code-line indent">  <span class="keyword">return</span> <span class="string">"Success"</span>;</div>
                                <div class="code-line">}</div>
                            </div>
                            <div class="code-bracket right">"}</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
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
    mounted() { },
    methods: {
        translate(key) {
            if (typeof shared !== 'undefined' && typeof shared.translate === 'function') {
                return shared.translate(key);
            }
            return key;
        },
        goToLogin() {
            window.location.href = "/AppEndStudio/";
        },
        scrollToFeatures() {
            const featuresSection = document.querySelector('#features-section');
            if (featuresSection) {
                featuresSection.scrollIntoView({ behavior: 'smooth' });
            }
        }
    },
    props: { cid: String }
}
</script>

<style scoped>
.hero-image-placeholder {
    min-height: 400px;
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative;
}

.hero-visual {
    display: flex;
    align-items: center;
    gap: 10px;
    font-family: 'Courier New', monospace;
    font-size: 1.5rem;
    color: rgba(255, 255, 255, 0.9);
    text-shadow: 0 2px 10px rgba(0,0,0,0.3);
}

.code-bracket {
    font-size: 4rem;
    font-weight: bold;
    opacity: 0.8;
    animation: pulse 2s ease-in-out infinite;
}

.code-content {
    background: rgba(255, 255, 255, 0.1);
    padding: 2rem;
    border-radius: 10px;
    backdrop-filter: blur(10px);
    border: 1px solid rgba(255, 255, 255, 0.2);
}

.code-line {
    margin: 0.5rem 0;
    line-height: 1.8;
}

.code-line.indent {
    padding-right: 2rem;
}

.keyword {
    color: #ffd700;
}

.function {
    color: #87ceeb;
}

.string {
    color: #98fb98;
}

@media (max-width: 768px) {
    .hero-visual {
        font-size: 1rem;
    }
    
    .code-bracket {
        font-size: 2.5rem;
    }
    
    .code-content {
        padding: 1rem;
    }
}
</style>

