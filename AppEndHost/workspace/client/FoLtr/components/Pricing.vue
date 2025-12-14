<template>
    <section id="pricing-section" class="section bg-light">
        <div class="container">
            <h2 class="section-title">{{ translate("Pricing") }}</h2>
            <div class="row g-4 justify-content-center">
                <div class="col-md-24 col-lg-16" v-for="(plan, index) in plans" :key="index">
                    <div class="card pricing-card h-100" :class="{ 'featured': plan.featured }">
                        <div class="card-body text-center p-4">
                            <div v-if="plan.featured" class="badge bg-primary mb-3">{{ translate("Pro") }}</div>
                            <h3 class="card-title mb-3">{{ translate(plan.name) }}</h3>
                            <div class="price mb-4">
                                <span class="price-amount">{{ plan.price }}</span>
                                <span class="price-period">{{ translate("per month") }}</span>
                            </div>
                            <ul class="list-unstyled mb-4 text-start">
                                <li class="mb-3" v-for="(feature, fIndex) in plan.features" :key="fIndex">
                                    <i class="fa-solid fa-fw fa-check text-success ms-2"></i>
                                    {{ translate(feature) }}
                                </li>
                            </ul>
                            <button class="btn w-100" :class="plan.featured ? 'btn-primary-custom text-white' : 'btn-outline-primary'" @click="handlePlanClick(plan.name)">
                                {{ translate(plan.buttonText) }}
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</template>

<script>
let _this = { 
    cid: "", 
    c: null,
    plans: [
        {
            name: "Free",
            price: "0",
            features: [
                "Up to 5 projects",
                "Basic templates",
                "Community support",
                "1GB storage"
            ],
            buttonText: "Get Started",
            featured: false
        },
        {
            name: "Pro",
            price: "99",
            features: [
                "Unlimited projects",
                "All templates",
                "Priority support",
                "50GB storage",
                "Advanced features"
            ],
            buttonText: "Get Started",
            featured: true
        },
        {
            name: "Enterprise",
            price: "Custom",
            features: [
                "Everything in Pro",
                "Dedicated support",
                "Custom integrations",
                "Unlimited storage",
                "SLA guarantee"
            ],
            buttonText: "Contact Sales",
            featured: false
        }
    ]
};

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
        handlePlanClick(planName) {
            if (planName === "Enterprise") {
                const contactSection = document.querySelector('#contact-section');
                if (contactSection) {
                    const offset = 80;
                    const elementPosition = contactSection.getBoundingClientRect().top;
                    const offsetPosition = elementPosition + window.pageYOffset - offset;
                    window.scrollTo({
                        top: offsetPosition,
                        behavior: 'smooth'
                    });
                }
            } else {
                window.location.href = "/AppEndStudio/";
            }
        }
    },
    props: { cid: String }
}
</script>

<style scoped>
.pricing-card {
    border: 2px solid #e9ecef;
    border-radius: 20px;
    transition: all 0.3s ease;
}

.pricing-card:hover {
    transform: translateY(-10px);
    box-shadow: 0 15px 40px rgba(0,0,0,0.1);
}

.pricing-card.featured {
    border-color: var(--fo-primary);
    box-shadow: 0 10px 30px rgba(102, 126, 234, 0.2);
}

.price-amount {
    font-size: 3rem;
    font-weight: 700;
    color: var(--fo-primary);
}

.price-period {
    font-size: 1rem;
    color: #6c757d;
    margin-left: 0.5rem;
}

.pricing-card .list-unstyled li {
    padding: 0.5rem 0;
}
</style>


